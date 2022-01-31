import { ApiException } from "./client";

export class ImageUploadClient
{
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> })
    {
        this.http = http ? http : window as any;
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    uploadProfilePicture(file: File): Promise<string>
    {
        let url = this.baseUrl + `/api/v1/upload/image`;
        url = url.replace(/[?&]$/, "");

        const content = new FormData();
        if (file !== null && file !== undefined)
        {
            content.append("file", file);
        }

        const options =
        {
            body: content,
            method: "POST",
            headers: {
                "Accept": "application/json"
            }
        } as RequestInit;

        return this.http.fetch(url, options).then((response: Response) =>
        {
            return this.processUpload(response);
        });
    }

    protected processUpload(response: Response): Promise<string>
    {
        const status = response.status;
        const _headers: any = {};

        if (response.headers && response.headers.forEach)
        {
            response.headers.forEach((v: any, k: any) => _headers[k] = v);
        }

        if (status === 200)
        {
            return response.text().then((_responseText) =>
            {
                let result200: any = null;
                result200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver) as string;
                return result200;
            });
        }
        else if (status === 400)
        {
            return response.text().then((_responseText) =>
            {
                return this.throwException("A server side error occurred.", status, _responseText, _headers);
            });
        }
        else if (status !== 200 && status !== 204)
        {
            return response.text().then((_responseText) =>
            {
                return this.throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        
        return Promise.resolve<string>(null as any);
    }

    private throwException = (message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): any =>
    {
        if (result !== null && result !== undefined)
        {
            throw result;
        }
        else
        {
            throw new ApiException(message, status, response, headers, null);
        }
    }
}
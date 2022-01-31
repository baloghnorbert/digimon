import { ImageUploadClient } from "./client.files";
import { DigimonClient } from "../client/client";

export class DigimonService extends DigimonClient
{
    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> })
    {
        super(baseUrl, http);
        this.jsonParseReviver = ReviveDateTime;
    }
}


export class PositionService extends PositionClient
{
    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> })
    {
        super(baseUrl, http);
        this.jsonParseReviver = ReviveDateTime;
    }
}

export class FileUploadService extends ImageUploadClient
{
    constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> })
    {
        super(baseUrl, http);
        this.jsonParseReviver = ReviveDateTime;
    }
}

// tslint:disable-next-line: max-classes-per-file
function ReviveDateTime(key: any, value: any): any
{
    const DATE_PREFIX = "@!Date";
    if (typeof value === "string" && value.startsWith(DATE_PREFIX))
    {
        const datePart = value.substr(DATE_PREFIX.length);
        const converted = new Date(datePart)
        return converted;
    }
    else if(value === null)
    {
        return undefined;
    }

    return value;
}
export interface FetchResultCallbacks
{
    success: (res: Response) => void;
    fail: (error: any) => void;
}
export class TimeoutError extends Error
{
    constructor(message?: string)
    {
        super(message)
    }

    protected isTimeoutError = true;

    static isTimeoutError(obj: any): obj is TimeoutError
    {
        return obj.isTimeoutError === true;
    }
}
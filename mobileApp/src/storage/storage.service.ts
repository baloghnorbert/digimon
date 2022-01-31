import { JwtKey } from "./storage.keys";

export class StorageService
{
  set = (key: JwtKey, value: any): void =>
  {
        const data: string = JSON.stringify(value);
        localStorage.setItem(key, data);
  }

  get = <T>(key: JwtKey): T | null =>
  {
    const data: any = localStorage.getItem(key);

    if (data && data !== "undefined" && data !== "null")
    {
        return JSON.parse(data) as T;
    }

    return null;
  }

  remove = (key: JwtKey): void =>
  {
    localStorage.removeItem(key);
  }

  clearStorage(): void
  {
    localStorage.clear();
  }
}
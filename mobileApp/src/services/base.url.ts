// locale
const localhost = "http://localhost:5000";

// production
const production = "";

// production or stage
export const baseURL: string = process.env.NODE_ENV === "production" ?  production : localhost;
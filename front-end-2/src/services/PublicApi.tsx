import * as axios from "axios";
import { Introduction, ExecutionResult } from "../models";

export default class PublicApi {

    private static init()
    {
      return axios.default.create({
        baseURL: process.env.REACT_APP_API_PUBLIC_ENDPOINT,
        timeout: 31000,
        headers: {
          Accept: "application/json",
        },
      });
    };
  
    public static async getIntroduction()
    {
        return await this.init().get<ExecutionResult<Introduction>>("/introduction");        
    };
}

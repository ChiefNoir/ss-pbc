import * as axios from "axios";
import { Introduction, ExecutionResult } from "../models/_index";
import { ProjectPreview } from "./models/_index"

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

    public static async getProjects(start: number, length: number, categoryCode: string | undefined)
    {
      const categoryParam = categoryCode !== undefined && categoryCode
      ? '&categorycode=' + categoryCode
      : '';

        return await this.init().get<ExecutionResult<Array<ProjectPreview>>>('projects/search?start=' + start +
        '&length=' + length + categoryParam);
    };
}

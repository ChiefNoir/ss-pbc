import * as axios from "axios";
import { Introduction, ExecutionResult } from "../models/_index";
import { Category, ProjectPreview } from "./models/_index"

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

    public static async getCategories()
    {
        return await this.init().get<ExecutionResult<Array<Category>>>("/categories");
    };

    public static async getProjects(page: number, categoryCode: string | undefined)
    {
      const length : number = parseInt(process.env.REACT_APP_PAGING_PROJECTS_MAX ?? '10');
      const start : number = length * (page - 1);

      const categoryParam = categoryCode !== undefined && categoryCode
      ? '&categorycode=' + categoryCode
      : '';

        return await this.init().get<ExecutionResult<Array<ProjectPreview>>>
        ('projects/search?'
        + 'start=' + start 
        + '&length=' + length 
        + categoryParam);
    };
}

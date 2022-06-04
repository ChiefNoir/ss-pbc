import * as axios from "axios";
import { Credentials } from ".";
import { ExecutionResult } from "./models/ExecutionResult";
import { Identity } from "./models/Identity";

class PrivateApi {
  private static init() {
    return axios.default.create({
      baseURL: process.env.REACT_APP_API_PRIVATE_ENDPOINT,
      timeout: 31000,
      headers: {
        Accept: "application/json",
        ContentType: "application/json"
      }
    });
  };

  public static async login(credentials: Credentials) {
    return await this.init().post<ExecutionResult<Identity>>("/login", credentials);
  };
}

export { PrivateApi };

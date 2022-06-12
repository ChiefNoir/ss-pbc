import * as axios from "axios";
import { Account, Category, Credentials, Information, Introduction } from ".";
import { store } from "../storage";
import { ExecutionResult } from "./models/ExecutionResult";
import { Identity } from "./models/Identity";

class PrivateApi {
  private static init() {
    const identity = store.getState().identity.value;

    return axios.default.create({
      baseURL: process.env.REACT_APP_API_PRIVATE_ENDPOINT,
      timeout: 31000,
      headers: {
        Accept: "application/json",
        ContentType: "application/json",
        Authorization: `Bearer ${identity?.token}`
      },
    });
  };

  public static async login(credentials: Credentials) {
    return await this.init().post<ExecutionResult<Identity>>("/login", credentials);
  };

  public static async getInformation() {
    return await this.init().get<ExecutionResult<Information>>("/information");
  }

  public static async getAccounts() {
    return await this.init().get<ExecutionResult<Account[]>>("/accounts");
  }

  public static async getRoles() {
    return await this.init().get<ExecutionResult<string[]>>("/roles");
  }

  public static async saveIntroduction(intro: Introduction) {
    return await this.init().post<ExecutionResult<Introduction>>("/introduction", intro);
  }

  public static async saveCategory(category: Category) {
    return await this.init().post<ExecutionResult<Category>>("/category", category);
  }

  public static async deleteCategory(category: Category) {
    return await this.init().delete<ExecutionResult<boolean>>("/category", { data: category });
  }

  public static async saveAccount(account: Account) {
    return await this.init().post<ExecutionResult<Account>>("/accounts", account);
  }

  public static async deleteAccount(account: Account) {
    return await this.init().delete<ExecutionResult<boolean>>("/accounts", { data: account });
  }
}

export { PrivateApi };

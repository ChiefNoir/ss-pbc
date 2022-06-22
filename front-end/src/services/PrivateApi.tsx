import * as axios from "axios";
import { Account, Category, Credentials, Introduction, Project } from ".";
import { store } from "../storage";
import { ExecutionResult } from "./models/ExecutionResult";
import { Identity } from "./models/Identity";
import { Utils } from "./Utils";

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
      }
    });
  };

  public static async login(credentials: Credentials) {
    return await this.init().post<ExecutionResult<Identity>>("/login", credentials)
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<Identity>(error);
      });
  };

  public static async getAccounts() {
    return await this.init().get<ExecutionResult<Account[]>>("/accounts")
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<Account[]>(error);
      });
  }

  public static async saveAccount(account: Account) {
    return await this.init().post<ExecutionResult<Account>>("/accounts", account)
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<Account>(error);
      });
  }

  public static async deleteAccount(account: Account) {
    return await this.init().delete<ExecutionResult<boolean>>("/accounts", { data: account })
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<boolean>(error);
      });
  }

  public static async getRoles() {
    return await this.init().get<ExecutionResult<string[]>>("/roles")
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<string[]>(error);
      });
  }

  public static async saveIntroduction(introduction: Introduction) {
    const intro = introduction;

    intro.externalUrls.forEach(x => {
      // Hack: fix it
      if (x.id != null && isNaN(+x.id)) {
        x.id = null;
      }
    });

    return await this.init().post<ExecutionResult<Introduction>>("/introduction", intro)
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<Introduction>(error);
      });
  }

  public static async saveCategory(category: Category) {
    return await this.init().post<ExecutionResult<Category>>("/category", category)
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<Category>(error);
      });
  }

  public static async deleteCategory(category: Category) {
    return await this.init().delete<ExecutionResult<boolean>>("/category", { data: category })
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<boolean>(error);
      });
  }

  public static async deleteProject(project: Project) {
    return await this.init().delete<ExecutionResult<boolean>>("/project", { data: project })
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<boolean>(error);
      });
  }

  public static async saveProject(project: Project) {
    const prj = project;

    // Hack: fix it
    if (prj.id !== null && isNaN(+prj.id)) {
      prj.id = null;
    }

    prj.externalUrls.forEach(x => {
      // Hack: fix it
      if (x.id != null && isNaN(+x.id)) {
        x.id = null;
      }
    });

    return await this.init().post<ExecutionResult<Project>>("/project", project)
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<Project>(error);
      });
  }

  public static async upload(file: Blob) {
    const formData = new FormData();
    formData.append("file", file);

    return await this.init().post<ExecutionResult<string>>("/upload", formData)
      .then(response => {
        return response.data;
      })
      .catch((error: axios.AxiosError) => {
        return Utils.create<string>(error);
      });
  }
}

export { PrivateApi };

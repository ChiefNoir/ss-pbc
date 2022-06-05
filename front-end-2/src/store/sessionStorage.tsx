import { Identity } from "../services";

class WebStorage {
  private static storage = localStorage;
  private static keyIdentity = "identity";

  public static SaveIdentity(identity: Identity | null) {
    try {
      if (identity === null) {
        this.storage.removeItem(this.keyIdentity);
        return;
      }

      this.storage.setItem(this.keyIdentity, JSON.stringify(identity));
    } catch (error) {
      console.log(error);
    }
  }

  public static LoadIdentity(): Identity | null {
    try {
      const result = this.storage.getItem(this.keyIdentity);

      if (result === null) { return null; }

      return JSON.parse(result);
    } catch (error) {
      return null;
    }
  }
}

export { WebStorage };

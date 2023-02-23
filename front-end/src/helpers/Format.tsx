import { i18n } from "../locales/i18n";

class Format {
  public static ToDate(value: Date | null): string {
    if (value === null) {
      return i18n.t("Project.ReleaseDateNull");
    }

    const dt = new Date(value);

    return `${dt.getFullYear()}
                / ${("0" + (dt.getMonth() + 1)).slice(-2)}
                / ${("0" + dt.getDate()).slice(-2)}`;
  }
}

export { Format };

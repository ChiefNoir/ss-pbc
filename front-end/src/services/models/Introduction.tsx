import { ExternalUrl } from "./ExternalUrl";

class Introduction {
  public title: string;
  public content: string | null;
  public posterUrl: string | null;
  public posterDescription: string;
  public externalUrls: ExternalUrl[];
  public version: number;

  // front-only
  public posterPreview: string;
  public posterNew: Blob | undefined;

  public constructor() {
    this.externalUrls = [];
  }
}

export { Introduction };

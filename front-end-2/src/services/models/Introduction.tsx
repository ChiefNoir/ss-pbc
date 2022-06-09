import { ExternalUrl } from "./ExternalUrl";

class Introduction {
  public title: string;
  public content: string;
  public posterUrl: string;
  public posterDescription: string;
  public externalUrls: Array<ExternalUrl>;
  public version: number;

  // front-only
  public posterPreview: string;
}

export { Introduction };

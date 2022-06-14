import { ExternalUrl } from "./ExternalUrl";
import { Category } from "./Category";

class Project {
  id: number | null;
  code: string;
  displayName: string;
  releaseDate: Date | null;
  posterUrl: string;
  posterDescription: string;
  category: Category;
  description: string;
  descriptionShort: string;

  externalUrls: ExternalUrl[];

  version: number;

  // front-only
  public posterPreview: string;

  public constructor() {
    this.externalUrls = [];
  }
}

export { Project };

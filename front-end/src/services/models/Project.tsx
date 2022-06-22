import { ExternalUrl } from "./ExternalUrl";
import { Category } from "./Category";

class Project {
  id: string | null;
  code: string;
  name: string;
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

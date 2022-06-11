import { ExternalUrl } from "./ExternalUrl";
import { Category } from "./Category";

export type Project = {
  id: number;
  code: string;
  displayName: string;
  releaseDate: Date;
  posterUrl: string;
  posterDescription: string;
  category: Category;
  description: string;
  descriptionShort: string;

  externalUrls: Array<ExternalUrl>;

  version: number;
}

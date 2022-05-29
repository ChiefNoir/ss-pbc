import { Category } from "./Category";

export type ProjectPreview = {
  code: string;
  displayName: string;
  releaseDate: Date;
  posterUrl: string;
  posterDescription: string;
  category: Category;
  description: string;
}

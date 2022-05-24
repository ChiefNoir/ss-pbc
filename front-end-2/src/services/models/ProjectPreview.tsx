import { Category } from './_index';

export type ProjectPreview = {
  id: number;
  code: string;
  displayName: string;
  releaseDate: Date;
  posterUrl: string;
  posterDescription: string;
  category: Category;
  description: string;
}

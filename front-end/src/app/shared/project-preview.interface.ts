import { Category } from './category.model';

export interface ProjectPreview {
  id: number;
  code: string;
  displayName: string;
  releaseDate: Date;
  posterUrl: string;
  posterDescription: string;
  category: Category;
  description: string;
}

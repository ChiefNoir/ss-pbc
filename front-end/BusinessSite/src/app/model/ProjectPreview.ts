import { Category } from 'src/app/model/Category';

export class ProjectPreview {
  public code: string;
  public displayName: string;
  public releaseDate: Date;
  public posterUrl: string;
  public posterDescription: string;
  public category: Category;
  public description: string;
}

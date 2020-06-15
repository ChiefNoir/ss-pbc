import { ExternalUrl } from 'src/app/model/ExternalUrl';
import { Category } from 'src/app/model/Category';

export class Project {
  public code: string;
  public displayName: string;
  public releaseDate: Date;
  public posterUrl: string;
  public posterDescription: string;
  public category: Category;
  public description: string;
  public descriptionShort: string;

  public externalUrls: Array<ExternalUrl>;

  public version: number;
}

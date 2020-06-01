import { ExternalUrl } from 'src/app/model/ExternalUrl';
import { Category } from 'src/app/model/Category';

export class Project {
    public id: number;

    public code: string;
    public displayName: string;
    public releaseDate: Date;
    public imageUrl: string;
    public categoryId: number;
    public category: Category;
    public descriptionShort: string;
    public description: string;

    public externalUrls: Array<ExternalUrl>;

    public version: number;
  }
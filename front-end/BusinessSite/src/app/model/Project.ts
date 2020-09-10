import { ExternalUrl } from 'src/app/model/ExternalUrl';
import { Category } from 'src/app/model/Category';
import { GalleryImage } from 'src/app/model/GalleryImage';

export class Project {
  public id: number;
  public code: string;
  public displayName: string;
  public releaseDate: Date;
  public posterUrl: string;
  public posterDescription: string;
  public category: Category;
  public description: string;
  public descriptionShort: string;

  public externalUrls: Array<ExternalUrl>;
  public galleryImages: Array<GalleryImage>;

  public posterToUpload: File;
  public posterPreview: string;

  public version: number;
}

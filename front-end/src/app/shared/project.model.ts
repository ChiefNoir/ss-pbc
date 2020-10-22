import { ExternalUrl } from '../shared/external-url.model';
import { Category } from '../shared/category.model';
import { GalleryImage } from '../shared/gallery-image.model';

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

  // [front-only]
  public posterPreview: string;

  public version: number;
}

import { ExternalUrl } from "./ExternalUrl";
import { Category } from "./Category";
import { GalleryImage } from "./GalleryImage";

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
  galleryImages: Array<GalleryImage>;

  version: number;
}

export class GalleryImage
{
    public id: number;
    public extraUrl: string;
    public imageUrl: string;
    public version: number;

    public readyToUpload: File;

    // [front-only]
    public localPreview: string;
}
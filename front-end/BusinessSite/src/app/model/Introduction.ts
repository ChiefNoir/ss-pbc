import { ExternalUrl } from './ExternalUrl';

export class Introduction {
    public title: string;
    public content: string;
    public posterUrl: string;
    public posterDescription: string;
    public version: number;

    public posterToUpload: File;
    public posterPreview: string;

    public externalUrls: Array<ExternalUrl>;
}

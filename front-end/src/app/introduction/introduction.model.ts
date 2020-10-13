import { ExternalUrl } from '../shared/external-url.model';

export class Introduction
{
    public title: string;
    public content: string;
    public posterUrl: string;
    public posterDescription: string;
    public externalUrls: Array<ExternalUrl>;
    public version: number;

    public posterToUpload: File;

    // [front-only]
    public posterPreview: string;
}

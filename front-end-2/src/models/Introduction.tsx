import { ExternalUrl } from './_index';

export type Introduction = {
  title: string;
  content: string;
  posterUrl: string;
  posterDescription: string;
  externalUrls: Array<ExternalUrl>;
  version: number;
}
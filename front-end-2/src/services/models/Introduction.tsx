import { ExternalUrl } from "./ExternalUrl";

export type Introduction = {
  title: string;
  content: string;
  posterUrl: string;
  posterDescription: string;
  externalUrls: Array<ExternalUrl>;
  version: number;
}

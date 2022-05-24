import { ExternalUrl } from './_index';

type Introduction = 
{
  title: string;
  content: string;
  posterUrl: string;
  posterDescription: string;
  externalUrls: Array<ExternalUrl>;
  version: number;
}

export default Introduction;
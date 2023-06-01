import { PageInfoModel } from './PageInfoModel';

export interface PageResponseModel<T> {
  data: T[];
  pageInfo: PageInfoModel;
}

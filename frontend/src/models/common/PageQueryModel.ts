import { QueryModel } from './QueryModel';

export interface PageQueryModel extends QueryModel {
  pageNumber?: number;
  itemsPerPage?: number;
}

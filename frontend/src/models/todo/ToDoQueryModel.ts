import { StatusType } from '../../enums/StatusType';
import { QueryModel } from '../common';

export type ToDoQueryModel = {
  status?: StatusType;
  dueAtStart?: Date;
  dueAtEnd?: Date;
} & QueryModel;

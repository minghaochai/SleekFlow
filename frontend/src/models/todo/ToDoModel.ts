import { StatusType } from '../../enums/StatusType';

export interface ToDoModel {
  id?: number;
  name?: string;
  description?: string;
  dueAt?: Date;
  status?: StatusType;
  addAt?: Date;
  addBy?: string;
  editAt?: Date;
  editBy?: string;
}

import {
  PageInfoModel,
  ToDoModel,
  ToDoQueryModel,
  UserModel,
} from '../../models';

export interface ToDoListState {
  isAdd: boolean;
  isLoading: boolean;
  addEditDialogOpen: boolean;
  rows: ToDoModel[];
  currentRow?: ToDoModel;
  query: ToDoQueryModel;
  pageInfo: PageInfoModel;
  user: UserModel;
}

export interface AddEditToDoDialogProps {
  open: boolean;
  close: () => void;
}

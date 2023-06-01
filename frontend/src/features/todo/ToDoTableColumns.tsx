import { GridCellParams, GridColDef } from '@mui/x-data-grid';
import { enqueueSnackbar } from 'notistack';
import { useTranslation } from 'react-i18next';

import { EditFilledIcon, IconButton, TrashFilledIcon } from '../../components';
import { StatusType } from '../../enums/StatusType';
import { ToDoModel } from '../../models';
import {
  useAppDispatch,
  useLazyGetToDosQueryListQuery,
  useRemoveToDoMutation,
  useStoreSelector,
} from '../../redux/store';
import { setAddEditDialogOpen, setCurrentRow, setIsAdd } from './ToDoListSlice';

export function UseColumns(): GridColDef[] {
  const { t } = useTranslation();
  const dispatch = useAppDispatch();
  const [removeToDoAsync] = useRemoveToDoMutation();
  const [getToDosAsync] = useLazyGetToDosQueryListQuery();
  const { pageInfo, query } = useStoreSelector((state) => state.toDoList);

  function renderEditButtonCell(params: GridCellParams<ToDoModel, any, any>) {
    const { row } = params;
    return (
      <IconButton
        onClick={() => {
          dispatch(setCurrentRow(row));
          dispatch(setIsAdd(false));
          dispatch(setAddEditDialogOpen(true));
        }}
      >
        <EditFilledIcon fill='#0B41CD' height={18} width={20} />
      </IconButton>
    );
  }

  function renderDeleteButtonCell(params: GridCellParams<ToDoModel, any, any>) {
    const { row } = params;
    return (
      <IconButton
        onClick={async () => {
          try {
            await removeToDoAsync({ id: row.id!.toString() }).unwrap();
            await getToDosAsync({
              params: {
                pageNumber: pageInfo.pageNumber,
                itemsPerPage: pageInfo.itemsPerPage,
                ...query,
              },
            });
            enqueueSnackbar(t('common.success'), {
              variant: 'success',
              autoHideDuration: 3000,
            });
          } catch {
            enqueueSnackbar(t('common.error'), {
              variant: 'error',
              autoHideDuration: 3000,
            });
          }
        }}
      >
        <TrashFilledIcon fill='#0B41CD' height={18} width={20} />
      </IconButton>
    );
  }

  return [
    {
      field: 'name',
      headerName: t('toDo.name') ?? '',
      width: 150,
      disableColumnMenu: true,
    },
    {
      field: 'description',
      headerName: t('toDo.description') ?? '',
      width: 250,
      sortable: false,
      disableColumnMenu: true,
    },
    {
      field: 'dueAt',
      headerName: t('toDo.dueAt') ?? '',
      width: 150,
      disableColumnMenu: true,
      valueFormatter: (params) => {
        const fullDate = params.value as Date;
        const year = fullDate.getFullYear();
        const month = fullDate.getMonth() + 1;
        const date = fullDate.getDate();
        return `${date < 10 ? `0${date}` : date}/${
          month < 10 ? `0${month}` : month
        }/${year}`;
      },
    },
    {
      field: 'status',
      headerName: t('toDo.status') ?? '',
      width: 150,
      disableColumnMenu: true,
      valueFormatter: (params) => {
        const value = params.value as StatusType;
        if (value === StatusType.NotStarted) {
          return 'Not Started';
        } else if (value === StatusType.InProgress) {
          return 'In Progress';
        } else if (value === StatusType.Completed) {
          return 'Completed';
        } else {
          return '-';
        }
      },
    },
    {
      field: 'editButton',
      headerName: '',
      width: 80,
      renderCell: (params: GridCellParams<any, ToDoModel, any>) =>
        renderEditButtonCell(params),
      sortable: false,
      disableColumnMenu: true,
    },
    {
      field: 'deleteButton',
      headerName: '',
      width: 80,
      renderCell: (params: GridCellParams<any, ToDoModel, any>) =>
        renderDeleteButtonCell(params),
      sortable: false,
      disableColumnMenu: true,
    },
  ];
}

export default UseColumns;

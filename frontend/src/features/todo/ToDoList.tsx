import { styled } from '@mui/material';
import { useEffect } from 'react';
import { useTranslation } from 'react-i18next';

import {
  Button,
  DataGrid,
  GridSortDirectionModel,
  Loading,
} from '../../components';
import { DefaultPageInfo } from '../../constants';
import { ToDoModel, ToDoQueryModel } from '../../models';
import {
  useAppDispatch,
  useLazyGetToDosQueryListQuery,
  useStoreSelector,
} from '../../redux/store';
import { AddEditToDoDialog } from './AddEditToDoDialog';
import { ToDoFilter } from './ToDoFilter';
import {
  setAddEditDialogOpen,
  setCurrentRow,
  setIsAdd,
  setQuery,
} from './ToDoListSlice';
import UseColumns from './ToDoTableColumns';

const StyledActionDiv = styled('div')({
  display: 'flex',
  columnGap: '20px',
  marginBottom: '20px',
});

export const ToDoList = () => {
  const { t } = useTranslation();
  const columns = UseColumns();
  const dispatch = useAppDispatch();
  const { rows, addEditDialogOpen, pageInfo, isLoading, query } =
    useStoreSelector((state) => state.toDoList);
  const [getToDosAsync] = useLazyGetToDosQueryListQuery();

  const openAddEditDialog = () => {
    const toDo: ToDoModel = {
      name: '',
      description: '',
      dueAt: new Date(),
      status: 0,
    };
    dispatch(setCurrentRow(toDo));
    dispatch(setIsAdd(true));
    dispatch(setAddEditDialogOpen(true));
  };

  const closeAddEditDialog = () => {
    dispatch(setAddEditDialogOpen(false));
  };

  const handlePaginationModelChange = (page: number, pageSize: number) => {
    getToDosAsync({
      params: {
        pageNumber: page + 1,
        itemsPerPage: pageSize,
        ...query,
      },
    });
  };

  const handleSortModelChange = (
    field: string | undefined,
    sort: GridSortDirectionModel,
  ) => {
    const updatedQuery: ToDoQueryModel = JSON.parse(JSON.stringify(query));
    updatedQuery.sortColumn = field;
    updatedQuery.sortDirection = sort ?? undefined;
    dispatch(setQuery(updatedQuery));
    getToDosAsync({
      params: {
        pageNumber: pageInfo.pageNumber,
        itemsPerPage: pageInfo.itemsPerPage,
        ...updatedQuery,
      },
    });
  };

  useEffect(() => {
    Promise.all([
      getToDosAsync({
        params: {
          pageNumber: DefaultPageInfo.pageNumber,
          itemsPerPage: DefaultPageInfo.itemsPerPage,
          ...query,
        },
      }),
    ]);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <>
      {isLoading && <Loading />}
      <StyledActionDiv>
        <ToDoFilter />
        <Button sx={{ marginLeft: 'auto' }} onClick={openAddEditDialog}>
          {t('toDo.add')}
        </Button>
      </StyledActionDiv>
      <DataGrid
        columns={columns}
        rows={rows}
        pageSize={pageInfo.itemsPerPage}
        currentPage={pageInfo.pageNumber}
        totalItems={pageInfo.totalItems}
        handlePaginationModelChange={handlePaginationModelChange}
        handleSortModelChange={handleSortModelChange}
      />
      {addEditDialogOpen && (
        <AddEditToDoDialog
          open={addEditDialogOpen}
          close={closeAddEditDialog}
        />
      )}
    </>
  );
};

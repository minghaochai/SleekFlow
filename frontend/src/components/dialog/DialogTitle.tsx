import { DialogTitle as MuiDialogTitle } from '@mui/material';

import { DialogTitleProps } from './types';

export function DialogTitle(props: DialogTitleProps) {
  return <MuiDialogTitle>{props.children}</MuiDialogTitle>;
}

export default DialogTitle;

import { ReactNode } from 'react';

export interface DialogProps {
  handleClose: () => void;
  open: boolean;
  children?: ReactNode;
}

export interface DialogTitleProps {
  children?: ReactNode;
}

export interface DialogContentProps {
  children?: ReactNode;
}

export interface DialogActionsProps {
  children?: ReactNode;
}

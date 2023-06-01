import { SxProps } from '@mui/material';
import { ReactNode } from 'react';

export interface ButtonProps {
  onClick?: () => void;
  children?: ReactNode;
  sx?: SxProps<any>;
}

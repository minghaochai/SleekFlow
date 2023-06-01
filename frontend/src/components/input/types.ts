import { SxProps, TextFieldVariants } from '@mui/material';
import { FocusEventHandler, ReactNode } from 'react';

import { OptionsModel } from '../../models';
export interface TextFieldProps {
  autoFocus?: boolean;
  id?: string;
  label?: string;
  type?: string;
  fullWidth?: boolean;
  variant?: TextFieldVariants;
  select?: boolean;
  defaultValue?: unknown;
  children?: ReactNode;
  value?: unknown;
  sx?: SxProps<any>;
  onBlur?: FocusEventHandler<HTMLInputElement | HTMLTextAreaElement>;
  onChange?: FocusEventHandler<HTMLInputElement | HTMLTextAreaElement>;
}

export type SelectFieldProps = {
  options: OptionsModel[];
} & TextFieldProps;

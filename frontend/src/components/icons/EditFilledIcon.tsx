import { SvgIcon, SvgIconProps } from '../svg-icon';

export function EditFilledIcon({ fill, ...props }: SvgIconProps) {
  const finalFill = fill ?? '#21201F';

  return (
    <SvgIcon height={24} width={24} viewBox='0 0 24 24' {...props}>
      <g clipPath='url(#clip0_2_1277)'>
        <path
          d='M18 21H6C4.3 21 3 19.7 3 18V6C3 4.3 4.3 3 6 3H13V5H6C5.4 5 5 5.4 5 6V18C5 18.6 5.4 19 6 19H18C18.6 19 19 18.6 19 18V11H21V18C21 19.7 19.7 21 18 21Z'
          fill={finalFill}
        />
        <path
          d='M18.8 3C19.1 3 19.3 3.09999 19.5 3.29999L20.7 4.5C20.9 4.7 21 4.90001 21 5.20001C21 5.50001 20.9 5.70002 20.7 5.90002L9.6 17H7V14.4L18.1 3.29999C18.3 3.09999 18.6 3 18.8 3Z'
          fill={finalFill}
        />
      </g>
      <defs>
        <clipPath id='clip0_2_1277'>
          <rect width='24' height='24' fill='white' />
        </clipPath>
      </defs>
    </SvgIcon>
  );
}

export default EditFilledIcon;

import { SvgIcon, SvgIconProps } from '../svg-icon';

export function TrashFilledIcon({ fill, ...props }: SvgIconProps) {
  const finalFill = fill ?? '#21201F';

  return (
    <SvgIcon height={24} width={24} viewBox='0 0 24 24' {...props}>
      <path
        d='M21 5H3v2h2v13a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V7h2ZM9 2h6v2H9z'
        fill={finalFill}
      ></path>
    </SvgIcon>
  );
}

export default TrashFilledIcon;

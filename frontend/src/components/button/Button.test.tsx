import { act, render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

import Button from './Button';

describe('Button component', () => {
  test('rendered button', () => {
    //arrange
    render(<Button>Test</Button>);

    //act
    const buttonTextElement = screen.getByText('Test');

    //assert
    expect(buttonTextElement).toBeInTheDocument();
  });

  test('onClick callback', async () => {
    //arrange
    console.log = jest.fn();
    const onClick = () => {
      console.log('clicked');
    };
    render(<Button onClick={onClick}>Test</Button>);

    //act
    const buttonElement = screen.getByRole('button');
    await act(async () => {
      await userEvent.click(buttonElement);
    });

    //assert
    expect(console.log).toHaveBeenCalledWith('clicked');
  });
});

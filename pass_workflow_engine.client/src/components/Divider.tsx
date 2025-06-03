interface DividerProps {
  width?: string;
  className?: string;
}

export function Divider({ width = '100%', className = '' }: DividerProps) {
  return (
    <hr
      style={{
        width,
        border: 'none',
        borderTop: '1px solid black',
        margin: '0'
      }}
      className={className}
    />
  );
} 
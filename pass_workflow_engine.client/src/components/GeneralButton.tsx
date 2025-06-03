import React, { ButtonHTMLAttributes, useCallback, useRef, useState } from 'react';
import GetJsonsButton from './GetJsonsButton';
import ParseModelButton from './ParseModelButton';
import { useWorkflow } from '../context/WorkflowContext';

// Types
type ButtonType = 'upload' | 'select' | 'parse';

interface GeneralButtonProps extends Omit<ButtonHTMLAttributes<HTMLButtonElement>, 'type'> {
  type: ButtonType;
  text: string;
  width?: number;
  height?: number;
}

// Styles
const styles = {
  container: {
    display: 'flex',
    flexDirection: 'column' as const,
    alignItems: 'center',
    gap: '1rem'
  },
  button: (width: number, height: number, disabled: boolean, isDragActive: boolean): React.CSSProperties => ({
    position: 'relative',
    width: `${width}px`,
    height: `${height}px`,
    padding: 0,
    border: 'none',
    background: 'none',
    cursor: disabled ? 'not-allowed' : 'pointer',
    opacity: disabled ? 0.7 : 1,
    transition: 'all 0.2s ease',
    outline: isDragActive ? '2px dashed #000' : 'none',
    outlineOffset: isDragActive ? '4px' : '0'
  }),
  content: {
    position: 'absolute' as const,
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    zIndex: 1,
    width: '100%',
    textAlign: 'center' as const,
    color: 'black',
    fontSize: '1.25rem',
    fontWeight: 500,
    padding: '0 2rem'
  },
  message: {
    base: {
      fontSize: '0.875rem',
      marginTop: '0.5rem',
      textAlign: 'center' as const,
      width: '100%'
    },
    error: { color: 'red' },
    success: { color: 'green' },
    info: { color: '#666' }
  },
  select: {
    width: '80%',
    padding: '0.5rem',
    marginTop: '1rem',
    fontSize: '1rem',
    borderRadius: '4px',
    border: '1px solid #ccc'
  },
  hiddenInput: {
    display: 'none'
  }
};

// SVG Button Component
const SVGButton: React.FC<{
  width: number;
  height: number;
  disabled?: boolean;
  isDragActive?: boolean;
  children: React.ReactNode;
  onClick?: () => void;
  onDrop?: (e: React.DragEvent) => void;
  onDragOver?: (e: React.DragEvent) => void;
  onDragEnter?: (e: React.DragEvent) => void;
  onDragLeave?: (e: React.DragEvent) => void;
}> = ({ 
  width, 
  height, 
  disabled, 
  isDragActive = false,
  children, 
  onClick,
  onDrop,
  onDragOver,
  onDragEnter,
  onDragLeave
}) => (
  <button 
    style={styles.button(width, height, !!disabled, isDragActive)} 
    disabled={disabled}
    onClick={onClick}
    onDrop={onDrop}
    onDragOver={onDragOver}
    onDragEnter={onDragEnter}
    onDragLeave={onDragLeave}
  >
    <svg
      width={width}
      height={height}
      viewBox={`0 0 ${width} ${height}`}
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
      style={{ position: 'absolute', top: 0, left: 0 }}
    >
      <rect
        x="0.5"
        y="0.5"
        width={width - 1}
        height={height - 1}
        rx={height / 2}
        fill="#EFEFEF"
      />
      <rect
        x="0.5"
        y="0.5"
        width={width - 1}
        height={height - 1}
        rx={height / 2}
        stroke="black"
        strokeOpacity="0.2"
      />
    </svg>
    <div style={styles.content}>{children}</div>
  </button>
);

// Main Component
export const GeneralButton: React.FC<GeneralButtonProps> = ({
  type,
  text,
  width = 401,
  height = 80,
  ...props
}) => {
  const { uploadStatus, models, selectedModel, handleFileUpload } = useWorkflow();
  const [isDragActive, setIsDragActive] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const handleDrop = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragActive(false);

    const files = Array.from(e.dataTransfer.files);
    if (files.length > 0) {
      handleFileUpload(files[0]);
    }
  }, [handleFileUpload]);

  const handleDragOver = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
  }, []);

  const handleDragEnter = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragActive(true);
  }, []);

  const handleDragLeave = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragActive(false);
  }, []);

  const handleClick = useCallback(() => {
    if (type === 'upload' && fileInputRef.current) {
      fileInputRef.current.click();
    }
  }, [type]);

  const handleFileSelect = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (files && files.length > 0) {
      handleFileUpload(files[0]);
    }
  }, [handleFileUpload]);

  const renderContent = () => {
    const contentWrapper = (children: React.ReactNode) => (
      <div style={{ width: '100%', height: '100%', display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
        {children}
      </div>
    );

    switch (type) {
      case 'upload':
        return contentWrapper(
          <>
            <SVGButton 
              width={width} 
              height={height}
              isDragActive={isDragActive}
              onClick={handleClick}
              onDrop={handleDrop}
              onDragOver={handleDragOver}
              onDragEnter={handleDragEnter}
              onDragLeave={handleDragLeave}
            >
              {text}
            </SVGButton>
            <input
              ref={fileInputRef}
              type="file"
              onChange={handleFileSelect}
              style={styles.hiddenInput}
              accept=".owl"
            />
            {uploadStatus && (
              <p style={{
                ...styles.message.base,
                ...(uploadStatus.includes('successfully') ? styles.message.success : styles.message.error)
              }}>
                {uploadStatus}
              </p>
            )}
          </>
        );

      case 'select':
        return contentWrapper(
          <>
            <SVGButton width={width} height={height}>
              {text}
            </SVGButton>
            {models && (
              <select style={styles.select}>
                <option value="">Select a model</option>
                {models.map(f => (
                  <option key={f.Path} value={f.Path}>{f.Name}</option>
                ))}
              </select>
            )}
          </>
        );

      case 'parse':
        return contentWrapper(
          <>
            <SVGButton width={width} height={height}>
              {text}
            </SVGButton>
            
          </>
        );
    }
  };

  return (
    <article style={styles.container}>
      {renderContent()}
    </article>
  );
}; 
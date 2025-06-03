import React from 'react';
import { useWorkflow } from '../context/WorkflowContext';

const DragAndDrop: React.FC = () => {
  const { setUploadStatus } = useWorkflow();

  const handleFileUpload = async (file: File) => {
    const formData = new FormData();
    formData.append('owlfile', file);

    try {
      const response = await fetch('/api/Main/UploadFile', {
        method: 'POST',
        body: formData,
      });

      if (response.ok) {
        const result = await response.text();
        setUploadStatus(`File uploaded successfully: ${result}`);
      } else {
        setUploadStatus('Failed to upload file.');
      }
    } catch (error) {
      console.error('Error uploading file:', error);
      setUploadStatus('An error occurred while uploading the file.');
    }
  };

  const handleDrop = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
    event.stopPropagation();
    const file = event.dataTransfer.files[0];
    if (file) handleFileUpload(file);
  };

  const handleDragOver = (event: React.DragEvent<HTMLDivElement>) => {
    event.preventDefault();
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) handleFileUpload(file);
  };

  const handleButtonClick = () => {
    document.getElementById('fileInput')?.click();
  };

  return (
    <div>
      <div
        onDrop={handleDrop}
        onDragOver={handleDragOver}
        onClick={handleButtonClick}
      >
        Drag & Drop file here or click to browse
      </div>
      <input
        id="fileInput"
        type="file"
        accept=".owl"
        style={{ display: 'none' }}
        onChange={handleInputChange}
      />
    </div>
  );
};

export default DragAndDrop;
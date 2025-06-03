import React, { createContext, useContext, useState, ReactNode } from 'react';

interface FileInfo {
  Name: string;
  Path: string;
}

interface WorkflowContextType {
  uploadStatus: string | null;
  setUploadStatus: (status: string | null) => void;
  selectedModel: FileInfo | null;
  setSelectedModel: (model: FileInfo | null) => void;
  models: FileInfo[] | null;
  setModels: (models: FileInfo[] | null) => void;
  handleFileUpload: (file: File) => void;
}

const WorkflowContext = createContext<WorkflowContextType | undefined>(undefined);

export function WorkflowProvider({ children }: { children: ReactNode }) {
  const [uploadStatus, setUploadStatus] = useState<string | null>(null);
  const [selectedModel, setSelectedModel] = useState<FileInfo | null>(null);
  const [models, setModels] = useState<FileInfo[] | null>(null);

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

  const value = {
    uploadStatus,
    setUploadStatus,
    selectedModel,
    setSelectedModel,
    models,
    setModels,
    handleFileUpload
  };

  return (
    <WorkflowContext.Provider value={value}>
      {children}
    </WorkflowContext.Provider>
  );
}

export function useWorkflow() {
  const context = useContext(WorkflowContext);
  if (context === undefined) {
    throw new Error('useWorkflow must be used within a WorkflowProvider');
  }
  return context;
} 
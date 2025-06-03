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
    try {
      if (!file.name.endsWith('.json')) {
        setUploadStatus('Error: Please upload a JSON file');
        return;
      }

      const formData = new FormData();
      formData.append('file', file);

      const response = await fetch('http://localhost:8080/upload', {
        method: 'POST',
        body: formData,
      });

      if (!response.ok) {
        throw new Error('Upload failed');
      }

      const result = await response.json();
      setUploadStatus('File uploaded successfully');
      
      // Refresh the models list after successful upload
      const modelsResponse = await fetch('http://localhost:8080/models');
      if (modelsResponse.ok) {
        const modelsData = await modelsResponse.json();
        setModels(modelsData);
      }
    } catch (error) {
      setUploadStatus('Error uploading file');
      console.error('Upload error:', error);
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
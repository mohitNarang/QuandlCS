﻿using System;
using System.Collections.Generic;
using System.Text;
using QuandlCS.Helpers;
using QuandlCS.Interfaces;
using QuandlCS.Types;

namespace QuandlCS.Requests
{
  /// <summary>
  /// The class to represent a download request from Quandl
  /// </summary>
  public class QuandlDownloadRequest : IQuandlGETRequestBuilder
  {
    #region Construction

    public QuandlDownloadRequest()
    {
      Reset(true);
    }

    #endregion

    #region IQuandlRequest Members

    /// <summary>
    /// The API key to use for the request
    /// </summary>
    public string APIKey
    {
      get;
      set;
    }

    /// <summary>
    /// Resets the download request object
    /// </summary>
    public void Reset(bool resetAPIKey)
    {
      if (resetAPIKey)
      {
        APIKey = string.Empty;
      }

      Datacode = new Datacode();
      StartDate = DateTime.MinValue;
      EndDate = DateTime.MinValue;
      Frequency = Frequencies.None;
      Transformation = Transformations.None;
      Truncation = 0;
      Format = FileFormats.CSV;
      Headers = true;
    }

    #endregion

    #region IQuandlGETRequestBuilder

    /// <summary>
    /// Get the download request string
    /// </summary>
    /// <returns>The download request string</returns>
    public string GetGETRequestString()
    {
      return CreateRequestString();
    }

    #endregion

    #region QuandlDownloadRequest Members

    /// <summary>
    /// The datacode 
    /// </summary>
    public Datacode Datacode
    {
      get 
      {
        return _datacode; 
      }

      set
      {
        if (_datacode != value)
        {
          _datacode = value;
        }
      }
    }

    /// <summary>
    /// The format to request the data
    /// </summary>
    public FileFormats Format
    {
      get 
      {
        return _format; 
      }

      set
      {
        if (_format != value)
        {
          _format = value;
        }
      }
    }

    /// <summary>
    /// The sort order to apply to the data
    /// </summary>
    public SortOrders Sort
    {
      get;
      set;
    }

    /// <summary>
    /// The transformation to be applied to the download request
    /// </summary>
    public Transformations Transformation
    {
      get;
      set;
    }

    /// <summary>
    /// The frequency of the data to request
    /// </summary>
    public Frequencies Frequency
    {
      get;
      set;
    }

    /// <summary>
    /// The truncation to apply to the data
    /// </summary>
    public int Truncation
    {
      get;
      set;
    }

    /// <summary>
    /// The start date of the period that data should be returned for
    /// </summary>
    public DateTime StartDate
    {
      get
      {
        return _startDate;
      }

      set
      {
        _startDate = value;
      }
    }

    /// <summary>
    /// The end date of the period that data should be returned for
    /// </summary>
    public DateTime EndDate
    {
      get
      {
        return _endDate;
      }

      set
      {
        _endDate = value;  
      }
    }

    /// <summary>
    /// Whether or not headers should be included in the CSV data
    /// </summary>
    public bool Headers
    {
      get;
      set;
    }

    #endregion

    #region Implementation
    
    private string CreateRequestString()
    {
      ValidateData();

      StringBuilder sb = new StringBuilder();
      sb.Append(Constants.APIDatasetsAddress)
        .Append('/')
        .Append(_datacode.GetDatacode('/'))
        .Append(TypeConverter.FileFormatToString(_format))
        .Append('?');      

      if (APIKey != string.Empty)
      {
        sb.Append(Constants.APIAuthorization)
          .Append(APIKey)
          .Append('&');
      }

      sb.Append(Constants.APIFrequency)
        .Append(TypeConverter.FrequencyToString(Frequency))
        .Append('&')
        .Append(Constants.APITransformation)
        .Append(TypeConverter.TransformationsToString(Transformation))
        .Append('&')
        .Append(Constants.APISortOrder)
        .Append(TypeConverter.SortOrdersToString(Sort));

      if (Truncation > 0)
      {
        sb.Append('&')
          .Append(Constants.APITruncation)
          .Append(Truncation);
      }

      if (_startDate != DateTime.MinValue)
      {
        sb.Append('&')
          .Append(Constants.APIStartDate)
          .Append(TypeConverter.DateToString(_startDate));
      }

      if (_endDate != DateTime.MinValue)
      {
        sb.Append('&')
          .Append(Constants.APIEndDate)
          .Append(TypeConverter.DateToString(_endDate));
      }

      if (Format == FileFormats.CSV)
      {
        sb.Append('&')
          .Append(Constants.APIHeader)
          .Append(!Headers);
      }

      return sb.ToString();
    }

    private void ValidateData()
    {
      if (string.IsNullOrWhiteSpace(_datacode.Source))
      {
        throw new InvalidOperationException("The datacode for this request does not have a Source specified.");
      }

      if (string.IsNullOrWhiteSpace(_datacode.Code))
      {
        throw new InvalidOperationException("The datacode for this request does not have a Code specified.");
      }
    }

    #endregion

    #region Fields

    private Datacode _datacode;
    private FileFormats _format;

    private DateTime _startDate;
    private DateTime _endDate;
        
    #endregion
  }
}

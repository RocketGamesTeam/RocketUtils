﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RocketUtils.Editor
{
        public sealed class CommandLineReader
        {
            //Config
            private const string CUSTOM_ARGS_PREFIX = "-Args:";
            private const char CUSTOM_ARGS_SEPARATOR = ';';

            public static string[] GetCommandLineArgs()
            {
                return Environment.GetCommandLineArgs();
            }

            public static string GetCommandLine()
            {
                string[] args = GetCommandLineArgs();

                if (args.Length > 0)
                {
                    return string.Join(" ", args);
                }
                else
                {
                    Debug.LogError(
                        "CommandLineReader.cs - GetCommandLine() - Can't find any command line arguments!");
                    return "";
                }
            }

            public static Dictionary<string, string> GetCustomArguments()
            {
                Dictionary<string, string> customArgsDict = new Dictionary<string, string>();
                string[] commandLineArgs = GetCommandLineArgs();
                string[] customArgs;
                string[] customArgBuffer;
                string customArgsStr = "";

                try
                {
                    customArgsStr = commandLineArgs.Single(row => row.Contains(CUSTOM_ARGS_PREFIX));
                }
                catch (Exception e)
                {
                    Debug.LogError(
                        "CommandLineReader.cs - GetCustomArguments() - Can't retrieve any custom arguments in the command line [" +
                        commandLineArgs + "]. Exception: " + e);
                    return customArgsDict;
                }

                customArgsStr = customArgsStr.Replace(CUSTOM_ARGS_PREFIX, "");
                customArgs = customArgsStr.Split(CUSTOM_ARGS_SEPARATOR);

                foreach (string customArg in customArgs)
                {
                    customArgBuffer = customArg.Split('=');
                    if (customArgBuffer.Length == 2)
                    {
                        customArgsDict.Add(customArgBuffer[0], customArgBuffer[1]);
                    }
                    else
                    {
                        Debug.LogWarning(
                            "CommandLineReader.cs - GetCustomArguments() - The custom argument [" + customArg +
                            "] seem to be malformed.");
                    }
                }

                return customArgsDict;
            }

            public static string GetCustomArgument(string argumentName)
            {
                Dictionary<string, string> customArgsDict = GetCustomArguments();

                if (customArgsDict.ContainsKey(argumentName))
                {
                    return customArgsDict[argumentName];
                }
                else
                {
                    Debug.LogError(
                        "CommandLineReader.cs - GetCustomArgument() - Can't retrieve any custom argument named [" +
                        argumentName + "] in the command line [" + GetCommandLine() + "].");
                    return "";
                }
            }
        }
}